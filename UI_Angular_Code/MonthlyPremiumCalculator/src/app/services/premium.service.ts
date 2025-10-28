import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Member, PremiumResponse, PremiumCalculation } from '../models/member.model';

@Injectable({
  providedIn: 'root'
})
export class PremiumService {
  private apiUrl = 'https://localhost:7038/api/premium';

  constructor(private http: HttpClient) { }

  calculatePremium(member: Member): Observable<PremiumResponse> {
    return this.http.post<PremiumResponse>(`${this.apiUrl}/calculate`, member);
  }

  getCalculationHistory(): Observable<PremiumCalculation[]> {
    return this.http.get<PremiumCalculation[]>(`${this.apiUrl}/history`);
  }
}