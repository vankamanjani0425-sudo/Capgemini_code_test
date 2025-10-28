import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';
import { Member, Occupation, PremiumResponse } from '../../models/member.model';
import { OccupationService } from '../../services/occupation.service';
import { PremiumService } from '../../services/premium.service';

@Component({
  selector: 'app-premium-calculator',
  templateUrl: './premium-calculator.component.html',
  styleUrls: ['./premium-calculator.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class PremiumCalculatorComponent implements OnInit, OnDestroy {
  premiumForm: FormGroup;
  occupations: Occupation[] = [];
  monthlyPremium: number = 0;
  calculationDetails: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private occupationService: OccupationService,
    private premiumService: PremiumService
  ) {
    this.premiumForm = this.createForm();
  }

  ngOnInit(): void {
    this.loadOccupations();
    this.setupFormChanges();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      ageNextBirthday: ['', [Validators.required, Validators.min(18), Validators.max(100)]],
      dateOfBirth: ['', [Validators.required]],
      occupation: ['', Validators.required],
      deathSumInsured: ['', [Validators.required, Validators.min(1000), Validators.max(10000000)]]
    });
  }

  loadOccupations(): void {
    this.occupationService.getOccupations()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => this.occupations = data,
        error: (error) => {
          console.error('Error loading occupations:', error);
          this.errorMessage = 'Failed to load occupations';
        }
      });
  }

  setupFormChanges(): void {
    this.premiumForm.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(() => {
        if (this.premiumForm.valid) {
          this.calculatePremium();
        }
      });

    this.premiumForm.statusChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(status => {
        if (status === 'INVALID') {
          this.monthlyPremium = 0;
          this.calculationDetails = '';
        }
      });
  }

  calculatePremium(): void {
    if (this.premiumForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      
      const memberData: Member = {
        ...this.premiumForm.value,
        dateOfBirth: new Date(this.premiumForm.get('dateOfBirth')?.value).toISOString()
      };

      this.premiumService.calculatePremium(memberData)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response: PremiumResponse) => {
            this.monthlyPremium = response.monthlyPremium;
            this.calculationDetails = response.calculationDetails;
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error calculating premium:', error);
            this.errorMessage = typeof error === 'string' ? error : 'Error calculating premium';
            this.monthlyPremium = 0;
            this.calculationDetails = '';
            this.isLoading = false;
          }
        });
    }
  }

  onSubmit(): void {
    if (this.premiumForm.valid) {
      this.calculatePremium();
    } else {
      this.markFormGroupTouched();
    }
  }

  onReset(): void {
    this.premiumForm.reset();
    this.monthlyPremium = 0;
    this.calculationDetails = '';
    this.errorMessage = '';
  }

  private markFormGroupTouched(): void {
    Object.keys(this.premiumForm.controls).forEach(key => {
      this.premiumForm.get(key)?.markAsTouched();
    });
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.premiumForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  getFieldError(fieldName: string): string {
    const field = this.premiumForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) return 'This field is required';
      if (field.errors['minlength']) return `Minimum length is ${field.errors['minlength'].requiredLength}`;
      if (field.errors['min']) return `Minimum value is ${field.errors['min'].min}`;
      if (field.errors['max']) return `Maximum value is ${field.errors['max'].max}`;
    }
    return '';
  }
}