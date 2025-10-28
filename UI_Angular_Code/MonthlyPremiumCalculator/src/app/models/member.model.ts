export interface Member {
  name: string;
  ageNextBirthday: number;
  dateOfBirth: string;
  occupation: string;
  deathSumInsured: number;
}

export interface Occupation {
  id: number;
  name: string;
  rating: string;
}

export interface PremiumResponse {
  monthlyPremium: number;
  calculationDetails: string;
  error?: string;
}

export interface PremiumCalculation {
  id: number;
  name: string;
  ageNextBirthday: number;
  dateOfBirth: string;
  occupation: string;
  deathSumInsured: number;
  monthlyPremium: number;
  calculatedDate: string;
}