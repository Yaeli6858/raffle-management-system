import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CategoryCreateDto, CategoryResponseDto, CategoryUpdateDto } from '../models/category-model'
import { GiftResponseDto } from '../models/gift-model';

@Injectable({ providedIn: 'root' })
export class CategoriesService {

  private readonly baseUrl = 'http://localhost:5072/api/Category';

  constructor(private http: HttpClient) {}

getAll(): Observable<CategoryResponseDto[]> {
  return this.http
    .get<CategoryResponseDto[]>(this.baseUrl)
}

  getById(id: number): Observable<CategoryResponseDto> {
    return this.http
      .get<CategoryResponseDto>(`${this.baseUrl}/${id}`)
  }

  create(dto: CategoryCreateDto): Observable<CategoryResponseDto> {
    return this.http
      .post<CategoryResponseDto>(this.baseUrl, dto)
  }

  update(id: number, dto: CategoryUpdateDto): Observable<CategoryResponseDto> {
    return this.http
      .put<CategoryResponseDto>(`${this.baseUrl}/${id}`, dto)
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }


}
