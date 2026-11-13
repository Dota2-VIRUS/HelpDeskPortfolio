import { DatePipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import {
  PRIORITY_LABELS,
  STATUS_LABELS,
  Ticket,
  TicketPriority,
  TicketStatus,
} from './ticket.model';
import { TicketService } from './ticket.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, DatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private readonly api = inject(TicketService);
  private readonly fb = inject(FormBuilder);

  readonly statusLabels = STATUS_LABELS;
  readonly priorityLabels = PRIORITY_LABELS;
  readonly statuses = Object.keys(STATUS_LABELS) as TicketStatus[];
  readonly priorities = Object.keys(PRIORITY_LABELS) as TicketPriority[];

  tickets = signal<Ticket[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);

  statusFilter: TicketStatus | '' = '';
  search = '';

  form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(2000)]],
    priority: ['Medium' as TicketPriority],
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set(null);
    this.api.getAll(this.statusFilter, this.search).subscribe({
      next: (items) => {
        this.tickets.set(items);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Não foi possível carregar os chamados. A API está rodando?');
        this.loading.set(false);
      },
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.api.create(this.form.getRawValue()).subscribe({
      next: () => {
        this.form.reset({ title: '', description: '', priority: 'Medium' });
        this.load();
      },
      error: () => this.error.set('Erro ao criar o chamado.'),
    });
  }

  changeStatus(ticket: Ticket, status: TicketStatus): void {
    this.api
      .update(ticket.id, {
        title: ticket.title,
        description: ticket.description,
        priority: ticket.priority,
        status,
      })
      .subscribe({
        next: () => this.load(),
        error: () => this.error.set('Erro ao atualizar o chamado.'),
      });
  }

  remove(ticket: Ticket): void {
    if (!confirm(`Excluir o chamado "${ticket.title}"?`)) return;
    this.api.delete(ticket.id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Erro ao excluir o chamado.'),
    });
  }
}
