import { Routes } from '@angular/router';
import { PremiumCalculatorComponent } from '../app/Component/premium-calculator.component/premium-calculator.component';

export const routes: Routes = [
  { path: '', component: PremiumCalculatorComponent },
  { path: 'calculator', component: PremiumCalculatorComponent },
  { path: '**', redirectTo: '' }
];