import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WinningResponseDto, WinningCreateDto } from '../models/winning-model';
import { RaffleStatusResponse } from '../models/winning-model';

@Injectable({
  providedIn: 'root',
})
export class WinningService {

  private readonly baseUrl = 'http://localhost:5072/api/winning';

  constructor(private http: HttpClient) {}

  // GET: api/winning
  getAll(): Observable<WinningResponseDto[]> {
    return this.http.get<WinningResponseDto[]>(this.baseUrl);
  }

  // GET: api/winning/{id}
  getById(id: number): Observable<WinningResponseDto> {
    return this.http.get<WinningResponseDto>(`${this.baseUrl}/${id}`);
  }

  // POST: api/winning
  create(dto: WinningCreateDto): Observable<WinningResponseDto> {
    return this.http.post<WinningResponseDto>(this.baseUrl, dto);
  }

  // PUT: api/winning/{id}
  update(id: number, dto: WinningCreateDto): Observable<WinningResponseDto> {
    return this.http.put<WinningResponseDto>(`${this.baseUrl}/${id}`, dto);
  }

  // DELETE: api/winning/{id}
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // GET: api/winning/total-income
  getTotalIncome(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/total-income`);
  }

  // GET: api/winning/doRaffle
  doRaffle(): Observable<WinningResponseDto[]> {
    return this.http.get<WinningResponseDto[]>(`${this.baseUrl}/doRaffle`);
  }

    getStatus(): Observable<RaffleStatusResponse> {
      return this.http.get<RaffleStatusResponse>(`${this.baseUrl}/statusIsFinished`);
      
    }
}