import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Occupation } from '../models/member.model';

@Injectable({
  providedIn: 'root'
})
export class OccupationService {
  private apiUrl = 'https://localhost:7038/api/Occupations';

  constructor(private http: HttpClient) { }

  getOccupations(): Observable<Occupation[]> {
    return this.http.get<Occupation[]>(this.apiUrl);
  }
}