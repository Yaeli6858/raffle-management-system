

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap, map } from 'rxjs';

import {
  CartAddDto,
  CartUpdateDto,
  CartItemResponseDto,
  CartCheckoutResponseDto
} from '../models/cart-model';

@Injectable({ providedIn: 'root' })
export class CartService {

  private readonly baseUrl = 'http://localhost:5072/api/Cart';

  // Single source of truth
  private readonly _cart$ = new BehaviorSubject<CartItemResponseDto[]>([]);
  readonly cart$ = this._cart$.asObservable();

  constructor(private http: HttpClient) {}

  // Initial load (called once per login)
  loadCart(): Observable<void> {
    return this.http
      .get<CartItemResponseDto[]>(`${this.baseUrl}/cart`)
      .pipe(
        tap(cart => this._cart$.next(cart)),
        map(() => void 0)
      );
  }

  // Add item to cart
  add(dto: CartAddDto): Observable<CartItemResponseDto> {
    return this.http.post<CartItemResponseDto>(this.baseUrl, dto).pipe(
      tap(item => {
        this._cart$.next([...this._cart$.value, item]);
      })
    );
  }

  // Update quantity
  updateQty(dto: CartAddDto): Observable<CartItemResponseDto> {
            console.log("comeing to update", dto);

    return this.http.put<CartItemResponseDto>(this.baseUrl, dto).pipe(
      tap(updated => {
                    console.log("update!", updated);

        const next = this._cart$.value.map(item =>
          item.purchaseId === updated.purchaseId ? updated : item
        );
        this._cart$.next(next);
        console.log("Cart updated:", next); 
      })
    );
  }

  // Remove item
  
  remove(purchaseId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${purchaseId}`).pipe(
      tap(() => {
        const next = this._cart$.value.filter(
          item => item.purchaseId !== purchaseId
        );
        this._cart$.next(next);
      })
    );
  }

  // Checkout
  checkout(): Observable<CartCheckoutResponseDto> {
    return this.http
      .post<CartCheckoutResponseDto>(`${this.baseUrl}/checkout`, null)
      .pipe(
        tap(() => {
          this._cart$.next([]); // clear cart after checkout
        })
      );
  }

  // Optional helpers (selectors)
  
  readonly totalItems$ = this.cart$.pipe(
    map(items => items.reduce((sum, i) => sum + i.qty, 0))
  );

  readonly totalPrice$ = this.cart$.pipe(
    map(items => items.reduce((sum, i) => sum + i.qty * i.giftPrice, 0))
  );
}
