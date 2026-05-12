
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, BehaviorSubject, map } from 'rxjs';
import { LoginDto, RegisterDto, LoginResponseDto, UserResponseDto } from '../models/auth-model';
import { jwtDecode } from 'jwt-decode';

export interface JwtPayload {
  sub: string;
  email: string;
  exp: number;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string;
}


const roleMap: Record<string, 'User' | 'Admin' | 'Donor'> = {
  User: 'User',
  Admin: 'Admin',
  Donor: 'Donor'
};

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5072/api/auth';

  // סטייט ריאקטיבי של התחברות
  private loggedInSubject = new BehaviorSubject<boolean>(this.isTokenValid());
  loggedIn$ = this.loggedInSubject.asObservable();

  constructor(private http: HttpClient) { }

  //     AUTH API

  login(dto: LoginDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/login`, dto);
  }

  register(dto: RegisterDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.apiUrl}/register`, dto);
  }

  //     TOKEN HANDLING

  saveToken(token: string): void {
    localStorage.setItem('token', token);
    this.loggedInSubject.next(true); //  מודיע לכל האפליקציה
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout(): void {
    localStorage.removeItem('token');
    this.loggedInSubject.next(false); //  מודיע לכל האפליקציה
  }

  //     USER INFO

  getCurrentUser(): Observable<UserResponseDto | null> {
    if (!this.isTokenValid()) return of(null); //  בדיקה מוקדמת

    const token = this.getToken();
    if (token === null) return of(null);

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      const user: UserResponseDto = {
        id: Number(decoded.sub),
        name: '',
        email: decoded.email,
        phone:'',
        city: '',
        address: '',
        role: role ? roleMap[role] || 'User' : 'User'
      };
      return of(user);
    } catch {
      return of(null);
    }
  }

  getCurrentUserId(): Observable<number | null> {
    return this.getCurrentUser().pipe(
      map(user => user ? user.id : null)
    );
  }


  //   ROLES

  getRole(): string | null {
    const token = this.getToken();
    if (token === null || !this.isTokenValid()) return null;

    const decoded = jwtDecode<JwtPayload>(token) as any;


    return (
      decoded.roles ??
      decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
      null
    );
  }

  isDonor$ = this.loggedIn$.pipe(
    map(() => this.getRole() === 'Donor')
  );

  isAdmin$ = this.loggedIn$.pipe(
    map(() => this.getRole() === 'Admin')
  );



  //     AUTH STATE

  isLoggedIn(): boolean {
    return this.loggedInSubject.value;
  }

  private isTokenValid(): boolean {

    const token = this.getToken();

    if (token === null) return false;

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const now = Math.floor(Date.now() / 1000);
      return decoded.exp > now;
    } catch {
      return false;
    }
  }
}
