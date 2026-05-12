import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { GiftResponseDto, GiftCreateDto, GiftUpdateDto, PriceSort, } from '../models/gift-model'
import { GiftPurchaseCountDto } from '../models/purchase-model';

@Injectable({ providedIn: 'root' })
export class GiftsService {

  private readonly baseUrl = 'http://localhost:5072/api/Gift';

  constructor(private http: HttpClient) { }

  getAll(
    sort: PriceSort,
    categoryId?: number | null,
    donorId?: number | null
  ): Observable<GiftResponseDto[]> {

    let params: any = { sort };

    if (categoryId != null) {
      params.categoryId = categoryId;
    }

    if (donorId != null) {
      params.donorId = donorId;
    }

    return this.http.get<GiftResponseDto[]>(this.baseUrl, { params });
  }


  getById(id: number): Observable<GiftResponseDto> {
    return this.http
      .get<GiftResponseDto>(`${this.baseUrl}/${id}`)
  }

  create(data: FormData): Observable<GiftResponseDto> {
    return this.http
      .post<GiftResponseDto>(this.baseUrl, data)
  }

  update(id: number, dto: FormData): Observable<GiftResponseDto> {
    return this.http
      .put<GiftResponseDto>(`${this.baseUrl}/${id}`, dto)
  }

  delete(id: number): Observable<void> {
    console.log("hi", id);

    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getGiftsByCategory(categoryId: number | null): Observable<GiftResponseDto[]> {
    return this.http.get<GiftResponseDto[]>(`${this.baseUrl}/ByCategory/${categoryId}`, {
    });
  }

  getPurchaseCountByGift(): Observable<GiftPurchaseCountDto[]> {
    return this.http.get<GiftPurchaseCountDto[]>(`${this.baseUrl}/purchaseCount`);
  }

}
