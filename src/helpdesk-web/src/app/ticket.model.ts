export type TicketStatus = 'Open' | 'InProgress' | 'Resolved';
export type TicketPriority = 'Low' | 'Medium' | 'High';

export interface Ticket {
  id: number;
  title: string;
  description: string;
  status: TicketStatus;
  priority: TicketPriority;
  createdAt: string;
  updatedAt: string | null;
}

export interface CreateTicket {
  title: string;
  description: string;
  priority: TicketPriority;
}

export type UpdateTicket = Pick<Ticket, 'title' | 'description' | 'status' | 'priority'>;

export const STATUS_LABELS: Record<TicketStatus, string> = {
  Open: 'Aberto',
  InProgress: 'Em andamento',
  Resolved: 'Resolvido',
};

export const PRIORITY_LABELS: Record<TicketPriority, string> = {
  Low: 'Baixa',
  Medium: 'Média',
  High: 'Alta',
};
