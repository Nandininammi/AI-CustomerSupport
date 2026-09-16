import { Component, ChangeDetectorRef, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ChatService } from './chat.service';

interface ChatMessage {
  role: 'user' | 'assistant';
  content: string;
}

@Component({
  selector: 'app-root',
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {

  private chatService = inject(ChatService);
  private cdr = inject(ChangeDetectorRef);

  userMessage: string = '';

  isLoading: boolean = false;

  messages: ChatMessage[] = [];

  sendMessage(): void {

    if (!this.userMessage.trim() || this.isLoading) {
      return;
    }

    const message = this.userMessage.trim();

    console.log('1. Sending message:', message);

    this.messages.push({
      role: 'user',
      content: message
    });

    this.userMessage = '';
    this.isLoading = true;

    console.log('2. isLoading:', this.isLoading);

    this.chatService.sendMessage(message).subscribe({

      next: (response) => {

        console.log('3. API RESPONSE:', response);
        console.log('4. ANSWER:', response.answer);

        this.messages.push({
          role: 'assistant',
          content: response.answer
        });

        this.isLoading = false;

        // Force Angular UI update
        this.cdr.detectChanges();

        console.log('5. isLoading:', this.isLoading);
      },

      error: (error) => {

        console.error('API ERROR:', error);

        this.messages.push({
          role: 'assistant',
          content: 'Sorry, I could not connect to the support server.'
        });

        this.isLoading = false;

        // Force Angular UI update
        this.cdr.detectChanges();

        console.log('6. isLoading after error:', this.isLoading);
      }

    });
  }
}