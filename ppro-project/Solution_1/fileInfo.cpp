/*
	Author: Pratik Mahesh Pradhan
	Date Edited: 26.02.2018
	Program: FileInfo class implementation.
	Description: Code that defines the function prototypes of class FileInfo.
*/

#include "fileInfo.h"
#include <iostream>		/* For use of input-output (cout,cin) streams. */

FileInfo::FileInfo()
{
	// Default values
	this->fileName = "";
	// By default both transaction id are active
	this->isTranIdActive = true;
	this->isMerchantTranIdActive = true;
	this->numberOfInvalidTransactions = 0;
	this->totalVolume = 0.0;
	this->matchedVolume = 0.0;
}

FileInfo::FileInfo(std::string input)
{
	this->fileName = input;
	// By default both transaction id are active
	this->isTranIdActive = true;
	this->isMerchantTranIdActive = true;
	// Default values
	this->numberOfInvalidTransactions = 0;
	this->totalVolume = 0.0;
	this->matchedVolume = 0.0;
}

FileInfo::~FileInfo()
{
	
}

double FileInfo::getMissingVolume() const
{
	return (this->totalVolume - this->matchedVolume);
}

void FileInfo::getReport()
{
	std::cout << std::endl;
	std::cout << "---------------------- \n";
	std::cout << this->fileName << std::endl;
	if (this->numberOfInvalidTransactions > 0)
		std::cout << "This file has " << this->numberOfInvalidTransactions << " unknown transactions.\n";
	std::cout << "Total volume: " << this->totalVolume << std::endl;
	std::cout << "Matched volume: " << this->matchedVolume << std::endl;
	std::cout << "Missing volume: " << this->getMissingVolume() << std::endl;
	
	std::cout << "---------------------- \n";
}

void FileInfo::printFileInformation()
{
	std::cout << std::endl;
	std::cout << "---------------------- \n";
	std::cout << "Filename: " << this->fileName << std::endl;
	std::cout << "Currently has unknown transactions = " << this->numberOfInvalidTransactions;
	std::cout << " and total volume: " << this->totalVolume << std::endl;
	for (unsigned int j = 0; j < this->informationList.size(); j++)
	{
		std::cout << "TranId: " << this->informationList[j].transactionId << "  ";
		std::cout << "MerTranId: " << this->informationList[j].merchantTransactionId << "  ";
		std::cout << "Amount: " << this->informationList[j].amount << "  ";
		std::cout << "State: " << this->informationList[j].state << std::endl;
	}
	
	std::cout << "---------------------- \n";
}