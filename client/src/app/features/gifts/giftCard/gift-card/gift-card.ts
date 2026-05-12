import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { CartItemQty } from '../cart-item-qty/cart-item-qty';
import { AdminOptions } from '../admin-options/admin-options';
import { GiftResponseDto } from '../../../../core/models/gift-model';
import { CardModule } from 'primeng/card';
import { CartService } from '../../../../core/services/cart-service';
import { GiftsService } from '../../../../core/services/gifts-service';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../../../core/services/auth-service';
import { Router } from '@angular/router';
import { NotificationService } from '../../../../core/services/notification-service';

@Component({
  selector: 'app-gift-card',
  imports: [CartItemQty, CardModule, ButtonModule],
  templateUrl: './gift-card.html',
  styleUrl: './gift-card.scss',
})
export class GiftCard {
  constructor(private cartService: CartService, private giftsService: GiftsService, private authService: AuthService, private router: Router, private notificationService: NotificationService ) { }

  @Input() gift!: GiftResponseDto;
  @Input() isAdmin: boolean = false;
  @Input() purchaseId: number | null = null;
  @Input() qty: number=0;


  @Output() render = new EventEmitter<boolean>();
  @Output() edit = new EventEmitter<GiftResponseDto>();



  onCountChange(count: number) {
    if (this.gift?.id === undefined) return;


    this.authService.getCurrentUserId().subscribe(userId => {
      console.log("userId", userId);
      if (userId === null) { this.notificationService.showError('User not logged in'); this.router.navigate(['/login']); return; }
      console.log('purchaseID', this.purchaseId);

      if (count === 0 && this.purchaseId) {
        this.cartService.remove(this.purchaseId).subscribe();
        return;
      }

      this.cartService.updateQty({
        giftId: this.gift.id,
        qty: count
      }).subscribe();
      this.notificationService.showSuccess('Cart updated successfully');
    });
  }

  onDelete() {
    console.log("onDelete!");
this.notificationService.confirmDelete(() => {
    this.giftsService.delete(this.gift.id).subscribe({
      next: gift => {
        this.notificationService.showSuccess('Gift deleted successfully!');
        this.render.emit(true);
      }
    });
  });
  }

  onEdit() {
    this.edit.emit(this.gift);     
  }

  get imageSrc(): string {

    return this.gift.imageUrl
      ? `http://localhost:5072${this.gift.imageUrl}`
      : 'http://localhost:5072/uploads/gifts/placeholder.jpg';
  }
}
