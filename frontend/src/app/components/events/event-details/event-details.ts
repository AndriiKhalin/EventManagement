import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { EventService } from '../../../core/services/event';
import { AuthService } from '../../../core/services/auth';
import { Event } from '../../../core/models/event.model';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './event-details.html',
  styleUrls: ['./event-details.css']
})
export class EventDetailsComponent implements OnInit {
  event: Event | null = null;
  isLoading = true;
  errorMessage = '';
  showDeleteConfirm = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private eventService: EventService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const eventId = this.route.snapshot.paramMap.get('id');
    if (eventId) {
      this.loadEvent(eventId);
    }
  }

  loadEvent(id: string): void {
    this.isLoading = true;
    this.eventService.getEventById(id).subscribe({
      next: (event) => {
        this.event = event;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoading = false;
      }
    });
  }

  onJoinEvent(): void {
    if (!this.event) return;

    this.eventService.joinEvent(this.event.id).subscribe({
      next: () => this.loadEvent(this.event!.id),
      error: (error) => alert(error.message)
    });
  }

  onLeaveEvent(): void {
    if (!this.event) return;

    this.eventService.leaveEvent(this.event.id).subscribe({
      next: () => this.loadEvent(this.event!.id),
      error: (error) => alert(error.message)
    });
  }

  onEditEvent(): void {
    if (!this.event) return;
    this.router.navigate(['/events', this.event.id, 'edit']);
  }

  onDeleteEvent(): void {
    this.showDeleteConfirm = true;
  }

  confirmDelete(): void {
    if (!this.event) return;

    this.eventService.deleteEvent(this.event.id).subscribe({
      next: () => {
        this.router.navigate(['/events']);
      },
      error: (error) => {
        alert(error.message);
        this.showDeleteConfirm = false;
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  formatTime(date: Date): string {
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  goBack(): void {
  this.router.navigate(['/events']);
}
}
