import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { EventService } from '../../../core/services/event';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './create-event.html',
  styleUrls: ['./create-event.css']
})
export class CreateEventComponent implements OnInit {
  eventForm: FormGroup;
  errorMessage = '';
  isLoading = false;
  minDate: string;

  constructor(
    private fb: FormBuilder,
    private eventService: EventService,
    private router: Router
  ) {
    // Set minimum date to today
    const today = new Date();
    this.minDate = today.toISOString().split('T')[0];

    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(2000)]],
      date: ['', [Validators.required]],
      time: ['', [Validators.required]],
      location: ['', [Validators.required, Validators.maxLength(300)]],
      capacity: ['', [Validators.min(1)]],
      isPublic: [true]
    });
  }

  ngOnInit(): void {
    // Set default date and time
    const now = new Date();
    const tomorrow = new Date(now);
    tomorrow.setDate(tomorrow.getDate() + 1);

    this.eventForm.patchValue({
      date: tomorrow.toISOString().split('T')[0],
      time: '14:00'
    });
  }

  onSubmit(): void {
    if (this.eventForm.invalid) {
      this.markFormGroupTouched(this.eventForm);
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formValue = this.eventForm.value;

    // Combine date and time
    const dateTime = new Date(`${formValue.date}T${formValue.time}`);

    // Check if date is in the past
    if (dateTime <= new Date()) {
      this.errorMessage = 'Event date and time cannot be in the past';
      this.isLoading = false;
      return;
    }

    const request = {
      title: formValue.title,
      description: formValue.description,
      startDateTime: dateTime,
      location: formValue.location,
      capacity: formValue.capacity ? parseInt(formValue.capacity) : null,
      isPublic: formValue.isPublic
    };

    this.eventService.createEvent(request).subscribe({
      next: (event) => {
        this.router.navigate(['/events', event.id]);
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoading = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/events']);
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
    });
  }

  get title() { return this.eventForm.get('title'); }
  get description() { return this.eventForm.get('description'); }
  get date() { return this.eventForm.get('date'); }
  get time() { return this.eventForm.get('time'); }
  get location() { return this.eventForm.get('location'); }
  get capacity() { return this.eventForm.get('capacity'); }
}
