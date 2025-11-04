import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, AbstractControl, ReactiveFormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { PremiumCalculatorService } from '../../Services/premium-calculator.service';
import { Occupation } from '../../Models/occupation.model';
import { PremiumRequest } from '../../Models/premium-request.model';
import { PremiumResponse } from '../../Models/premium-response.model';

@Component({
  selector: 'app-premium-calculator',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './premium-calculator.component.html',
  styleUrls: ['./premium-calculator.component.scss']
})
export class PremiumCalculatorComponent implements OnInit, OnDestroy {
  premiumForm: FormGroup;
  occupations: Occupation[] = [];
  premiumResult: PremiumResponse | null = null;
  isLoading = false;
  errorMessage: string | null = null;
  
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private premiumService: PremiumCalculatorService
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

  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      ageNextBirthday: ['', [Validators.required, Validators.min(18), Validators.max(100)]],
      dateOfBirth: ['', [Validators.required, this.dateOfBirthValidator]],
      occupation: ['', Validators.required],
      deathSumInsured: ['', [Validators.required, Validators.min(1000), Validators.max(10000000)]]
    });
  }

  private dateOfBirthValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) {
      return null;
    }

    const pattern = /^(0[1-9]|1[0-2])\/\d{4}$/;
    if (!pattern.test(control.value)) {
      return { invalidFormat: true };
    }

    const [month, year] = control.value.split('/');
    const inputDate = new Date(parseInt(year), parseInt(month) - 1);
    const today = new Date();
    
    if (inputDate > today) {
      return { futureDate: true };
    }

    return null;
  }

  private loadOccupations(): void {
    this.isLoading = true;
    this.premiumService.getOccupations()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (occupations) => {
          this.occupations = occupations;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading occupations:', error);
          this.errorMessage = 'Failed to load occupations. Please try again.';
          this.isLoading = false;
        }
      });
  }

  private setupFormChanges(): void {
    // Trigger premium calculation when occupation changes and form is valid
    this.premiumForm.get('occupation')?.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(() => {
        if (this.premiumForm.valid) {
          this.calculatePremium();
        }
      });

    // Also trigger calculation when other critical fields change
    ['ageNextBirthday', 'deathSumInsured'].forEach(field => {
      this.premiumForm.get(field)?.valueChanges
        .pipe(
          takeUntil(this.destroy$),
          debounceTime(300),
          distinctUntilChanged()
        )
        .subscribe(() => {
          if (this.premiumForm.valid && this.premiumForm.get('occupation')?.value) {
            this.calculatePremium();
          }
        });
    });
  }

  calculatePremium(): void {
    if (this.premiumForm.valid) {
      this.isLoading = true;
      this.errorMessage = null;

      const formValue = this.premiumForm.value;
      const request: PremiumRequest = {
        name: formValue.name,
        ageNextBirthday: parseInt(formValue.ageNextBirthday),
        dateOfBirth: formValue.dateOfBirth,
        occupation: formValue.occupation,
        deathSumInsured: parseFloat(formValue.deathSumInsured)
      };

      this.premiumService.calculatePremium(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.premiumResult = response;
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error calculating premium:', error);
            this.errorMessage = error.message || 'Failed to calculate premium. Please check your inputs.';
            this.premiumResult = null;
            this.isLoading = false;
          }
        });
    }
  }

  onReset(): void {
    this.premiumForm.reset();
    this.premiumResult = null;
    this.errorMessage = null;
  }

  // Helper methods for template
  get name() { return this.premiumForm.get('name'); }
  get ageNextBirthday() { return this.premiumForm.get('ageNextBirthday'); }
  get dateOfBirth() { return this.premiumForm.get('dateOfBirth'); }
  get occupation() { return this.premiumForm.get('occupation'); }
  get deathSumInsured() { return this.premiumForm.get('deathSumInsured'); }
}



