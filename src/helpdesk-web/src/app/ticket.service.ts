import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateTicket, Ticket, TicketStatus, UpdateTicket } from './ticket.model';

@Injectable({ providedIn: 'root' })
export class TicketService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/tickets';

  getAll(status?: TicketStatus | '', search?: string): Observable<Ticket[]> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    if (search?.trim()) params = params.set('search', search.trim());
    return this.http.get<Ticket[]>(this.baseUrl, { params });
  }

  create(ticket: CreateTicket): Observable<Ticket> {
    return this.http.post<Ticket>(this.baseUrl, ticket);
  }

  update(id: number, ticket: UpdateTicket): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.baseUrl}/${id}`, ticket);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
