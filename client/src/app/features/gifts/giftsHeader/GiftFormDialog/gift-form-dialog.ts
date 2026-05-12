import { Component, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { GiftsService } from '../../../../core/services/gifts-service';
import { GiftResponseDto } from '../../../../core/models/gift-model';
import { CommonModule } from '@angular/common';
import { CategoryResponseDto } from '../../../../core/models/category-model';
import { DonorListItem } from '../../../../core/models/donor-model';
import { SelectModule } from 'primeng/select';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextModule } from 'primeng/inputtext';
import { FileSelectEvent, FileUploadModule } from 'primeng/fileupload';
import { NotificationService } from '../../../../core/services/notification-service';
import { ValidationErrorDirective } from '../../../../shared/directives/validation-error';
@Component({
  selector: 'app-gift-form-dialog',
  standalone: true,
  imports: [InputTextModule, FileUploadModule, DialogModule, ButtonModule, ReactiveFormsModule, CommonModule, FormsModule, SelectModule, InputNumberModule,ValidationErrorDirective],
  templateUrl: './gift-form-dialog.html',
  styleUrl: './gift-form-dialog.scss',
})
export class GiftFormDialog implements OnChanges {

  @Input() giftToEdit: GiftResponseDto | null = null;
  @Input() categories: CategoryResponseDto[] = [];
  @Input() donors: DonorListItem[] = [];

  @Output() created = new EventEmitter<boolean>();
  @Output() closed = new EventEmitter<void>();

  form: FormGroup;
  showDialog = false;
  editMode = false;

  selectedFile: File | null = null;  
  previewUrl: string | null = null;    

  constructor(private fb: FormBuilder, private giftsService: GiftsService, private notificationService: NotificationService ) {
    this.form = this.fb.group({
      description: ['', Validators.required],
      price: [null, [Validators.required, Validators.min(10), Validators.pattern('^[0-9]*$'),]],
      categoryId: [null, Validators.required],
      donorId: [null, Validators.required],
    });
  }



  ngOnChanges(changes: SimpleChanges): void {
    if (changes['giftToEdit'] && this.giftToEdit) {
      this.prepareEditMode(this.giftToEdit);
      this.showDialog = true;
    }
  }


  private prepareCreateMode(): void {
    this.editMode = false;
    this.form.reset({
      price: 10
    });
    this.selectedFile = null;
    this.previewUrl = null; 
  }

  private prepareEditMode(gift: GiftResponseDto): void {
    this.editMode = true;
    this.form.reset();

    this.selectedFile = null; 
    console.log(gift.imageUrl, 'giftimageurl1');
        console.log(this.previewUrl, 'previewurl1');

    this.previewUrl = gift.imageUrl ? `http://localhost:5072${gift.imageUrl}` : null;
    console.log(gift.imageUrl, 'giftimageurl2');

    console.log(this.previewUrl, 'previewurl2');
    

    this.form.patchValue({
      description: gift.description,
      price: gift.price,
      categoryId: gift.categoryId,
      donorId: gift.donorId,
    });
  }



  open(): void {
    this.prepareCreateMode();
    this.showDialog = true;
  }

  close(): void {
    this.showDialog = false;
    this.closed.emit();
  }



  onFileChange(event: FileSelectEvent): void {
    const file = event.files?.[0] ?? null;
    if (file === null) return;

    this.selectedFile = file;
    this.previewUrl = URL.createObjectURL(file);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const formData = new FormData();
    formData.append('description', this.form.value.description || '');
    formData.append('price', this.form.value.price.toString());
    formData.append('categoryId', this.form.value.categoryId.toString());
    formData.append('donorId', this.form.value.donorId.toString());

    if (this.selectedFile) {               
      formData.append('image', this.selectedFile);
    }

    const request$ = this.editMode
      ? this.giftsService.update(this.giftToEdit!.id, formData)
      : this.giftsService.create(formData);

    request$.subscribe({
      next: () => {
        this.created.emit(true);
        if(!this.editMode) {
          this.notificationService.showSuccess('Gift created successfully');
        } else {
          this.notificationService.showSuccess('Gift edited successfully');
        }
        this.close();
      },
    });
  }
}
