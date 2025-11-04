import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { PremiumCalculatorComponent } from '../app/Component/premium-calculator.component/premium-calculator.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    PremiumCalculatorComponent
  ],
  template: `
    <div class="app-container">
      <header class="app-header">
        <h1>Insurance Premium Calculator</h1>
      </header>
      <main class="app-main">
      <app-premium-calculator></app-premium-calculator>
      </main>
      <footer class="app-footer">
        <p>&copy; 2024 Insurance Company. All rights reserved.</p>
      </footer>
    </div>
  `,
  styles: [`
    .app-container {
      min-height: 100vh;
      display: flex;
      flex-direction: column;
    }
    .app-header {
      background-color: #1976d2;
      color: white;
      padding: 1rem;
      text-align: center;
    }
    .app-main {
      flex: 1;
      padding: 2rem;
    }
    .app-footer {
      background-color: #f5f5f5;
      padding: 1rem;
      text-align: center;
      border-top: 1px solid #ddd;
    }
  `]
})
export class App {
  title = 'premium-calculator';
}