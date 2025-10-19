export interface Event {
  id: string;
  title: string;
  description: string;
  startDateTime: Date;
  location: string;
  capacity: number | null;
  isPublic: boolean;
  participantCount: number;
  isFull: boolean;
  isUserParticipant: boolean;
  isUserOrganizer: boolean;
  organizer: {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    fullName: string;
  };
  participants: Participant[];
}

export interface Participant {
  userId: string;
  fullName: string;
  email: string;
  joinedAt: Date;
}

export interface CreateEventRequest {
  title: string;
  description: string;
  startDateTime: Date;
  location: string;
  capacity: number | null;
  isPublic: boolean;
}

export interface UpdateEventRequest {
  title: string;
  description: string;
  startDateTime: Date;
  location: string;
  capacity: number | null;
  isPublic: boolean;
}
