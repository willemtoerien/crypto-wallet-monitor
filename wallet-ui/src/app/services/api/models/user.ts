import { Transaction } from './transaction';

export interface User {
  email?: string;
  balance?: number;

  transactions: Transaction[];
}
