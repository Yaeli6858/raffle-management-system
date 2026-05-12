import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { GiftPurchaseCountDto, PurchaseCreateDto } from '../models/purchase-model';
import { PurchaseUpdateDto } from '../models/purchase-model';
import { PurchaseResponseDto } from '../models/purchase-model';

@Injectable({ providedIn: 'root' })
export class PurchaseService {

  private readonly baseUrl = 'http://localhost:5072/api/Purchase';

  constructor(private http: HttpClient) {}

  // Admin בלבד 
  getAll(): Observable<PurchaseResponseDto[]> {
    return this.http.get<PurchaseResponseDto[]>(this.baseUrl);
  }

  getById(id: number): Observable<PurchaseResponseDto> {
    return this.http.get<PurchaseResponseDto>(`${this.baseUrl}/${id}`);
  }

  create(dto: PurchaseCreateDto): Observable<PurchaseResponseDto> {
    return this.http.post<PurchaseResponseDto>(this.baseUrl, dto);
  }

  update(id: number, dto: PurchaseUpdateDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getByGift(giftId: number): Observable<PurchaseResponseDto[]> {
    return this.http.get<PurchaseResponseDto[]>(
      `${this.baseUrl}/byGift/${giftId}`
    );
  }

  getPurchaseCountByGift(): Observable<GiftPurchaseCountDto[]> {
    return this.http.get<GiftPurchaseCountDto[]>(`${this.baseUrl}/count-by-gift`);
  }
}