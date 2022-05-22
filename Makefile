NAME := computorv1

all: $(NAME)

$(NAME):
	dotnet build

clean:
	dotnet clean

test:
	./test.sh

fclean: clean

re: fclean all

