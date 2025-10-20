import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { EventService } from '../../../core/services/event';
import { Event } from '../../../core/models/event.model';

interface CalendarDay {
  date: Date;
  dayNumber: number;
  isCurrentMonth: boolean;
  events: Event[];
}

@Component({
  selector: 'app-my-events-calendar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './my-events-calendar.html',
  styleUrls: ['./my-events-calendar.css']
})
export class MyEventsCalendarComponent implements OnInit {
  events: Event[] = [];
  calendarDays: CalendarDay[] = [];
  currentDate = new Date();
  viewMode: 'month' | 'week' = 'month';
  isLoading = true;
  errorMessage = '';

  weekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];

  constructor(
    private eventService: EventService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadEvents();
  }

  loadEvents(): void {
    this.isLoading = true;
    this.eventService.getMyEvents().subscribe({
      next: (events) => {
        this.events = events;
        this.generateCalendar();
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = error.message;
        this.isLoading = false;
      }
    });
  }

  generateCalendar(): void {
    if (this.viewMode === 'month') {
      this.generateMonthView();
    } else {
      this.generateWeekView();
    }
  }

  generateMonthView(): void {
    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();

    const firstDay = new Date(year, month, 1);
    const lastDay = new Date(year, month + 1, 0);
    const startDate = new Date(firstDay);
    startDate.setDate(startDate.getDate() - startDate.getDay());

    this.calendarDays = [];
    const currentDate = new Date(startDate);

    while (currentDate <= lastDay || this.calendarDays.length % 7 !== 0) {
      const dayEvents = this.getEventsForDate(currentDate);

      this.calendarDays.push({
        date: new Date(currentDate),
        dayNumber: currentDate.getDate(),
        isCurrentMonth: currentDate.getMonth() === month,
        events: dayEvents
      });

      currentDate.setDate(currentDate.getDate() + 1);
    }
  }

  generateWeekView(): void {
    const startOfWeek = new Date(this.currentDate);
    startOfWeek.setDate(startOfWeek.getDate() - startOfWeek.getDay());

    this.calendarDays = [];
    const currentDate = new Date(startOfWeek);

    for (let i = 0; i < 7; i++) {
      const dayEvents = this.getEventsForDate(currentDate);

      this.calendarDays.push({
        date: new Date(currentDate),
        dayNumber: currentDate.getDate(),
        isCurrentMonth: true,
        events: dayEvents
      });

      currentDate.setDate(currentDate.getDate() + 1);
    }
  }

  getEventsForDate(date: Date): Event[] {
    return this.events.filter(event => {
      const eventDate = new Date(event.startDateTime);
      return eventDate.toDateString() === date.toDateString();
    });
  }

  previousPeriod(): void {
    if (this.viewMode === 'month') {
      this.currentDate.setMonth(this.currentDate.getMonth() - 1);
    } else {
      this.currentDate.setDate(this.currentDate.getDate() - 7);
    }
    this.currentDate = new Date(this.currentDate);
    this.generateCalendar();
  }

  nextPeriod(): void {
    if (this.viewMode === 'month') {
      this.currentDate.setMonth(this.currentDate.getMonth() + 1);
    } else {
      this.currentDate.setDate(this.currentDate.getDate() + 7);
    }
    this.currentDate = new Date(this.currentDate);
    this.generateCalendar();
  }

  setViewMode(mode: 'month' | 'week'): void {
    this.viewMode = mode;
    this.generateCalendar();
  }

  navigateToEvent(eventId: string): void {
    this.router.navigate(['/events', eventId]);
  }

  navigateToCreateEvent(): void {
    this.router.navigate(['/events/create']);
  }

  get currentMonthYear(): string {
    return this.currentDate.toLocaleDateString('en-US', {
      month: 'long',
      year: 'numeric'
    });
  }

  isToday(date: Date): boolean {
    const today = new Date();
    return date.toDateString() === today.toDateString();
  }

  formatTime(date: Date): string {
    return new Date(date).toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: false
    });
  }
}
