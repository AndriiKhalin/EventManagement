import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EventService } from '../../../core/services/event';
import { Event } from '../../../core/models/event.model';

@Component({
  selector: 'app-edit-event',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './edit-event.html',
  styleUrls: ['./edit-event.css']
})
export class EditEventComponent implements OnInit {
  eventForm: FormGroup;
  event: Event | null = null;
  errorMessage = '';
  isLoading = false;
  isLoadingEvent = true;
  minDate: string;

  constructor(
    private fb: FormBuilder,
    private eventService: EventService,
    private route: ActivatedRoute,
    private router: Router
  ) {
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
    const eventId = this.route.snapshot.paramMap.get('id');
    if (eventId) {
      this.loadEvent(eventId);
    }
  }

  loadEvent(id: string): void {
    this.isLoadingEvent = true;
    this.eventService.getEventById(id).subscribe({
      next: (event) => {
        this.event = event;
        this.populateForm(event);
        this.isLoadingEvent = false;
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoadingEvent = false;
      }
    });
  }

  populateForm(event: Event): void {
    const eventDate = new Date(event.startDateTime);
    const date = eventDate.toISOString().split('T')[0];
    const time = eventDate.toTimeString().slice(0, 5);

    this.eventForm.patchValue({
      title: event.title,
      description: event.description,
      date: date,
      time: time,
      location: event.location,
      capacity: event.capacity,
      isPublic: event.isPublic
    });
  }

  onSubmit(): void {
    if (this.eventForm.invalid || !this.event) {
      this.markFormGroupTouched(this.eventForm);
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formValue = this.eventForm.value;
    const dateTime = new Date(`${formValue.date}T${formValue.time}`);

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

    this.eventService.updateEvent(this.event.id, request).subscribe({
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
    if (this.event) {
      this.router.navigate(['/events', this.event.id]);
    } else {
      this.router.navigate(['/events']);
    }
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
