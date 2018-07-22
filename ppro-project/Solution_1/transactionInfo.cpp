/*
	Author: Pratik Mahesh Pradhan
	Date Edited: 26.02.2018
	Program: Transaction Information Implementation File.
	Description: Implementation for transaction info class.
*/

#include "transactionInfo.h"

TransactionInfo::TransactionInfo()
{
	// Default Values for transaction
	this->transactionId = 0;	
	this->merchantTransactionId = 0;	
	this->amount = 0.0;
	this->state = FAILED;
	this->isVerified = false;
}

TransactionInfo::~TransactionInfo()
{
	
}