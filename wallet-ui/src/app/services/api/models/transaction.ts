import { User } from './user';

export interface Transaction {
  transactionId?: string;
  fromUserId?: number;
  toUserId?: number;
  amount?: number;
  purpose?: string;
  transactionAt?: string;
  isSuspicious?: boolean;

  fromUser?: User;
  toUser?: User;
}
