import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment';
import { Event, CreateEventRequest, UpdateEventRequest } from '../models/event.model';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private apiUrl = `${environment.apiUrl}/events`;

  constructor(private http: HttpClient) {}

  getPublicEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(this.apiUrl);
  }

  getEventById(id: string): Observable<Event> {
    return this.http.get<Event>(`${this.apiUrl}/${id}`);
  }

  getMyEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.apiUrl}/my-events`);
  }

  createEvent(request: CreateEventRequest): Observable<Event> {
    return this.http.post<Event>(this.apiUrl, request);
  }

  updateEvent(id: string, request: UpdateEventRequest): Observable<Event> {
    return this.http.patch<Event>(`${this.apiUrl}/${id}`, request);
  }

  deleteEvent(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  joinEvent(id: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${id}/join`, {});
  }

  leaveEvent(id: string): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/${id}/leave`, {});
  }
}
