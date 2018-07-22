/*
	Author: Pratik Mahesh Pradhan
	Date Edited: 26.02.2018
	Program: Transaction Information Header File.
	Description: This header file describes the necessary fields or attributes and methods required for a transaction.
*/

#ifndef TRANSACTION_INFO_H
#define TRANSACTION_INFO_H

enum TransactionStatus
{
	SUCCESS = 1,
	FAILED = 2,
	PENDING = 3
};

class TransactionInfo
{
	public:
		unsigned int transactionId;		
		unsigned int merchantTransactionId;		
		double amount;
		TransactionStatus state;
		bool isVerified;
		
		TransactionInfo();
		~TransactionInfo();	
};

#endif