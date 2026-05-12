import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DonorListItem, DonorDashboardResponse, RoleEnum, addDonorDto, DonorWithGiftsDto } from '../models/donor-model';

@Injectable({
  providedIn: 'root'
})
export class DonorService {

  private readonly baseUrl = 'http://localhost:5072/api/Donor';

  constructor(private http: HttpClient) {}

  // Admin

  /** GET api/Donor?search=&city= */
  getDonors(search?: string, city?: string): Observable<DonorListItem[]> {
    let params = new HttpParams();

    if (search)
      params = params.set('search', search);

    if (city)
      params = params.set('city', city);

    return this.http.get<DonorListItem[]>(this.baseUrl, { params });
  }

  getDonorsWithGifts(): Observable<DonorWithGiftsDto[]> {
    return this.http.get<DonorWithGiftsDto[]>(`${this.baseUrl}/with-gifts`);
  }

  /** PATCH api/Donor/role/{userId}?role= */
  setUserRole(userId: number, role: RoleEnum): Observable<void> {
    return this.http.patch<void>(
      `${this.baseUrl}/role/${userId}`,
      null,
      { params: { role } }
    );
  }

  // Donor

  /** GET api/Donor/dashboard */
  getMyDashboard(): Observable<DonorDashboardResponse> {
    return this.http.get<DonorDashboardResponse>(
      `${this.baseUrl}/dashboard`
    );
  }

  /** GET api/Donor/details */
  getMyDetails(): Observable<DonorListItem> {
    return this.http.get<DonorListItem>(
      `${this.baseUrl}/details`
    );
  }

  addDonor(dto:addDonorDto): Observable<DonorListItem> {
    return this.http.post<DonorListItem>(this.baseUrl, dto);
  }
}
