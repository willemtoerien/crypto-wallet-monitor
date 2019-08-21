import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Transaction, PayRequest } from './models';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  constructor(private http: HttpClient) {}

  getOutgoingTransactions(userId: number) {
    return this.http.get<Transaction[]>(`/transactions/${userId}/out`);
  }

  getIncomingTransactions(userId: number) {
    return this.http.get<Transaction[]>(`/transactions/${userId}/in`);
  }

  pay(request: PayRequest) {
    return this.http.post<void>(`/transactions/pay`, request);
  }
}
