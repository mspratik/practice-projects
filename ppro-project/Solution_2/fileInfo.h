/*
	Author: Pratik Mahesh Pradhan
	Date Edited: 26.02.2018
	Program: File Information storing unit.
	Description: This header file describes the necessary classes and methods for File Information.
*/

#ifndef FILE_INFO_H
#define FILE_INFO_H

#include <string>
#include <vector>
#include "transactionInfo.h"

class FileInfo
{
	public:
		std::string fileName;
		bool isTranIdActive;
		bool isMerchantTranIdActive;
		std::vector<TransactionInfo> informationList;
		unsigned int numberOfInvalidTransactions;
		double totalVolume;
		double matchedVolume;
		
		FileInfo();
		FileInfo(std::string input);
		~FileInfo();
		double getMissingVolume() const;
		void getReport();
		void printFileInformation();
};

#endif