import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { EventService } from '../../../core/services/event';
import { AuthService } from '../../../core/services/auth';
import { Event } from '../../../core/models/event.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './event-list.html',
  styleUrls: ['./event-list.css']
})
export class EventListComponent implements OnInit {
  events: Event[] = [];
  isLoading = true;
  errorMessage = '';
  isAuthenticated = false;
  searchTerm = '';

  constructor(
    private eventService: EventService,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;
    this.eventService.getPublicEvents().subscribe({
      next: (events) => {
        this.events = events;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoading = false;
      }
    });
  }

  onJoinEvent(eventId: string, event: MouseEvent): void {
    event.stopPropagation();

    if (!this.isAuthenticated) {
      this.router.navigate(['/auth/login']);
      return;
    }

    this.eventService.joinEvent(eventId).subscribe({
      next: () => this.loadEvents(),
      error: (error) => alert(error.message)
    });
  }

  onLeaveEvent(eventId: string, event: MouseEvent): void {
    event.stopPropagation();

    this.eventService.leaveEvent(eventId).subscribe({
      next: () => this.loadEvents(),
      error: (error) => alert(error.message)
    });
  }

  navigateToEvent(eventId: string): void {
    this.router.navigate(['/events', eventId]);
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  formatTime(date: Date): string {
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  get filteredEvents(): Event[] {
    if (!this.searchTerm) return this.events;

    const term = this.searchTerm.toLowerCase();
    return this.events.filter(event =>
      event.title.toLowerCase().includes(term) ||
      event.description.toLowerCase().includes(term) ||
      event.location.toLowerCase().includes(term)
    );
  }
}
