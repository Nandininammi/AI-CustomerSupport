import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ChatRequest {
  message: string;
}

export interface ChatResponse {
  answer: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  private apiUrl = 'https://localhost:7125/api/Chat';

  constructor(private http: HttpClient) {}

  sendMessage(message: string): Observable<ChatResponse> {
    return this.http.post<ChatResponse>(
      this.apiUrl,
      { message: message }
    );
  }
}