import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Occupation } from '../Models/occupation.model';
import { PremiumRequest } from '../Models/premium-request.model';
import { PremiumResponse } from '../Models/premium-response.model';

@Injectable({
  providedIn: 'root'
})
export class PremiumCalculatorService {
  private apiUrl = 'https://localhost:7184/api/Premium'; //NEED TO UPDATE BASED ON ACTUAL API URL

  constructor(private http: HttpClient) { }

  getOccupations(): Observable<Occupation[]> {
    console.log('Fetching occupations from API');
    return this.http.get<Occupation[]>(`${this.apiUrl}/occupations`)
      .pipe(
        tap(occupations => console.log('Occupations fetched successfully:', occupations)),
        catchError(this.handleError)
      );
  }

  calculatePremium(request: PremiumRequest): Observable<PremiumResponse> {
    console.log('Calculating premium for request:', request);
    return this.http.post<PremiumResponse>(`${this.apiUrl}/calculate`, request)
      .pipe(
        tap(response => console.log('Premium calculated successfully:', response)),
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('API Error occurred:', error);
    
    let errorMessage = 'An unexpected error occurred.';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.error && error.error.error) {
        errorMessage = error.error.error;
      } else {
        errorMessage = `Server returned code: ${error.status}, error message: ${error.message}`;
      }
    }
    
    return throwError(() => new Error(errorMessage));
  }
}