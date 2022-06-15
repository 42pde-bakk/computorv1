all:
	dotnet build

clean:
	dotnet clean

test:
	./test.sh

fclean: clean
	/bin/rm -rf bin obj

re: fclean all

.PHONY: all clean test fclean re
