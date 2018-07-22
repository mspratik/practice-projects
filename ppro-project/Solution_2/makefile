OBJECTS = transactionInfo.o fileInfo.o main.o
TARGET = ppro

$(TARGET): $(OBJECTS)
	g++ $(OBJECTS) -o $(TARGET)

%.o: %.cpp
	g++ -c $*.cpp

transactionInfo.o: transactionInfo.cpp transactionInfo.h

fileInfo.o: fileInfo.cpp fileInfo.h transactionInfo.h

main.o: main.cpp fileInfo.h transactionInfo.h
