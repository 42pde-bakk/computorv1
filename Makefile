all:
	dotnet build

clean:
	dotnet clean

test:
	./test.sh

fclean: clean
	/bin/rm -rf bin obj $(NAME)

re: fclean all

.PHONY: all $(NAME) clean test fclean re
