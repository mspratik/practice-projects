/*
	Author: Pratik Mahesh Pradhan
	Date Edited: 26.02.2018
	Program: Main implementation of PPRO.
	Description: Code to perform reconciliation.
*/

#include "transactionInfo.h"
#include "fileInfo.h"
#include "dirent.h"

#include <cstring>
#include <vector>
#include <sstream>
#include <fstream>
#include <algorithm>
#include <iostream>		/* For use of input-output (cout,cin) streams. */
#include <cstdlib>		/* For use of function exit, atoi. */

/*******************************/
// Method to parse transaction state.
/*******************************/
TransactionStatus getState(const std::string& input)
{
	TransactionStatus result = FAILED;
	if (input == "SUCCESS")
		result = SUCCESS;
	else if (input == "PENDING")
		result = PENDING;
		
	return result;
}

/*******************************/
// Method to split given string by delimiter.
/*******************************/
std::vector<std::string> split(const std::string& input, char delimiter)
{
   std::vector<std::string> tokens;
   std::string token;
   std::istringstream tokenStream(input);
   while (std::getline(tokenStream, token, delimiter))
   {
      tokens.push_back(token);
   }
   return tokens;
}

/*******************************/
// Method to read input having values separated by delimiter.
/*******************************/
int readDelimitedFile(FileInfo& outputFile, const char* path, char delimiter)
{
	int readStatus = 0;
	char filePath[1024];
	std::strcpy(filePath, path);
	std::strcat(filePath, outputFile.fileName.c_str());
	
	// Open Input File
	std::ifstream file(filePath);
	
	// Check if file open
	if (file.is_open())
	{
		std::string line = "";
		bool isHeaderDone = false;		
		std::vector<std::string> list;
		while(std::getline(file, line))
		{
			list = split(line, delimiter);
			// Read header provided
			if (!isHeaderDone)
			{			
				if (list.size() <= 2) // Input File cannot have lesser than 2 columns
				{
					std::cerr << "File has less information, Cannot proceed!! \n";
					readStatus = -1;
					break;
				}
				else if (list.size() == 3) // Deactivate respective ID
				{
					outputFile.isTranIdActive = (list[0] == "transactionId");
					outputFile.isMerchantTranIdActive = (list[0] == "merchantTransactionId");
				}
								
				isHeaderDone = true;
			}
			else // Collect transaction information
			{
				TransactionInfo info;
				bool transactionError = false;
				
				// Read in Transaction ID
				if(outputFile.isTranIdActive)
				{
					if (list[0] != "" && (atoi(list[0].c_str()) > 0))
						info.transactionId = atoi(list[0].c_str());
					else
						transactionError = true;
				}
				
				// Read in Merchant Transaction ID
				unsigned int index = (list.size() == 4) ? 1 : 0;
				if (outputFile.isMerchantTranIdActive)
				{
					if (list[index] != "" && (atoi(list[index].c_str()) > 0))
						info.merchantTransactionId = atoi(list[index].c_str());
					else
						transactionError = true;
				}
				
				// Read in State
				if (list[index + 1] != "")
					info.state = getState(list[index + 1]);
				else
					transactionError = true;
				
				// Read in Amount
				if (list[index + 2] != "")
				{
					info.amount = atof(list[index + 2].c_str());
					outputFile.totalVolume += info.amount;
				}
				else
					transactionError = true;
								
				if (transactionError)
				{
					info.isVerified = true;
					outputFile.numberOfInvalidTransactions++;
				}
				
				outputFile.informationList.push_back(info);
			}
			
			list.clear();
		}
		
		// Close file
		file.close();
	}
	else
	{
		std::cerr << "Unable to open file!! \n";
		readStatus = -1;
	}

	return readStatus;
}

/*******************************/
// Method to get input files for PPRO reconciliation process.
/*******************************/
int getInputFiles(std::vector<std::string> &input, const char* pproFilePath, const char* merchantFilePath, const std::string& fileType)
{
	int status = 0;
	
	// Open directory for PPRO file
	DIR* dir = opendir(pproFilePath);
    struct dirent* contents;
	if (dir != NULL)
	{
		while ((contents = readdir(dir)) != NULL) 
		{
			if (std::strcmp(contents->d_name, ".") != 0 && std::strcmp(contents->d_name, "..") != 0)
				input.push_back(contents->d_name);
		}
		
		// Close directory
		closedir(dir);
	}
	else
	{
		std::cerr << "\n Incorrect directory for PPRO file! \n";
		status = -1;
	}    
	
	if (status == 0)
	{
		// Open directory for Merchant files
		dir = opendir(merchantFilePath);
		if (dir != NULL)
		{
			while ((contents = readdir(dir)) != NULL) 
			{
				if (std::strcmp(contents->d_name, ".") != 0 && std::strcmp(contents->d_name, "..") != 0)
					input.push_back(contents->d_name);
			}
			
			// Close directory
			closedir(dir);
		}
		else
		{
			std::cerr << "\n Incorrect directory for Merchant files! \n";
			status = -1;
		}
		
		if (status == 0)
		{
			// Filter by file type	
			std::size_t match;
			for(std::vector<std::string>::iterator it = input.begin(); it != input.end(); ++it)
			{		
				match = (*it).find(fileType);
				if (match == std::string::npos)
					input.erase(it);
			}
		}		
	}		
	
	if (status == 0 && input.size() == 0)
	{
		std::cerr << "\n Unable to locate files under given directory! \n";
		status = -1;
	}
	
	return status;
}


int main(int argc, char* argv[])
{
	int programStatus = 0;
	
	// Process command line arguments
	char pproFilePath[1024];
	char merchantFilePath[1024];
	for (int k = 1; k < argc; k++) 
	{
        if (argv[k] == std::string("-P") || argv[k] == std::string("--path")) 
		{
			std::strcpy(pproFilePath, argv[k + 1]);
            k++;
			std::cout << "-> Given PPRO File Path:  " << pproFilePath << std::endl;
        }
		else if (argv[k] == std::string("-FP") || argv[k] == std::string("--file-path"))
		{
			std::strcpy(merchantFilePath, argv[k + 1]);
            k++;
			std::cout << "-> Given Merchant File Path:  " << merchantFilePath << std::endl;
		}
		else
		{
			std::cerr << "\n Unknown input argument: " << argv[k] << std::endl;
			programStatus = -1;
		}
	}
		
	if (programStatus == 0)
	{
		// Get Input files
		std::vector<std::string> inputFileList;
		programStatus = getInputFiles(inputFileList, pproFilePath, merchantFilePath, ".csv");
		
		if (programStatus == 0)
		{
			// File list to collect information of all given input files
			std::vector<FileInfo> fileList;
			// Reading files
			for (unsigned int f = 0; f < inputFileList.size(); f++)
			{
				// Get file information
				FileInfo fileObj(inputFileList[f]);
				programStatus = readDelimitedFile(fileObj, (f == 0) ? pproFilePath : merchantFilePath, ',');
				
				if (f == 0 && programStatus != 0)
				{
					std::cerr << "\n Unable to read PPRO file! Program Stopped!! \n";
					break;
				}
				else
				{
					if (programStatus == 0)
					{
						// Print and check file information
						fileObj.printFileInformation();
						
						// Add to list of files
						fileList.push_back(fileObj);
					}
					else
					{
						std::cerr << "\n Unable to read -> " << inputFileList[f] << " <- \n";
					}
				}				
			}
			
			if (programStatus == 0)
			{
				// Reconciliation Process Begins
				for(unsigned int i = 1; i < fileList.size(); i++)
				{										
					// Browse through transactions of current merchant file
					for (unsigned int j = 0; j < fileList[i].informationList.size(); j++)
					{											
						if (!(fileList[i].informationList[j].isVerified))
						{
							bool found = false;
							// Browse through transactions of PPRO file
							for (unsigned int k = 0; k < fileList[0].informationList.size(); k++)
							{
								if (fileList[i].isTranIdActive && fileList[i].isMerchantTranIdActive && fileList[i].informationList[j].transactionId == fileList[0].informationList[k].transactionId && 
								fileList[i].informationList[j].merchantTransactionId == fileList[0].informationList[k].merchantTransactionId &&	fileList[i].informationList[j].state == fileList[0].informationList[k].state &&	fileList[i].informationList[j].amount == fileList[0].informationList[k].amount)
								{
									found = true;
									fileList[i].matchedVolume += fileList[i].informationList[j].amount;
									fileList[i].informationList[j].isVerified = true;
									fileList[0].matchedVolume += fileList[0].informationList[k].amount;
									fileList[0].informationList[k].isVerified = true;
									break;
								}
								else if (fileList[i].isTranIdActive && fileList[i].informationList[j].transactionId == fileList[0].informationList[k].transactionId && fileList[i].informationList[j].state == fileList[0].informationList[k].state && fileList[i].informationList[j].amount == fileList[0].informationList[k].amount)
								{
									found = true;
									fileList[i].matchedVolume += fileList[i].informationList[j].amount;
									fileList[i].informationList[j].isVerified = true;
									fileList[0].matchedVolume += fileList[0].informationList[k].amount;
									fileList[0].informationList[k].isVerified = true;
									break;
								}
								else if (fileList[i].isMerchantTranIdActive && fileList[i].informationList[j].merchantTransactionId == fileList[0].informationList[k].merchantTransactionId && fileList[i].informationList[j].state == fileList[0].informationList[k].state && fileList[i].informationList[j].amount == fileList[0].informationList[k].amount)
								{
									found = true;
									fileList[i].matchedVolume += fileList[i].informationList[j].amount;
									fileList[i].informationList[j].isVerified = true;
									fileList[0].matchedVolume += fileList[0].informationList[k].amount;
									fileList[0].informationList[k].isVerified = true;
									break;
								}
							}
							
							if (!found)
								fileList[i].numberOfInvalidTransactions++;
						}					
					}					
				}
			
				// Report reconciliation results
				// Merchant Details 
				for(unsigned int i = 1; i < fileList.size(); i++)
				{
					fileList[i].getReport();
				}
				
				// PPRO Details
				fileList[0].getReport();
			}
		}		
	}
		
	return programStatus;
}