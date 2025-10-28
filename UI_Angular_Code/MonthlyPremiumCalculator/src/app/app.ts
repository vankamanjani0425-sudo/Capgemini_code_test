import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PremiumCalculatorComponent } from './components/premium-calculator.component/premium-calculator.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true,
  imports: [CommonModule, PremiumCalculatorComponent]
})
export class App {
  title = 'Premium Calculator';
}

