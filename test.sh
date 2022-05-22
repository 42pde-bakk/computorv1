#!/usr/bin/env bash

EXE="bin/Debug/net6.0/computorv1"

function run_testcase() {
	EQUATION=$1

	echo "Testing $EQUATION"
	$EXE "$EQUATION"
	echo
}

#run_testcase "5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0" # -9.3X^2 + 4X + 4 = 0
#run_testcase "5 * X^0 + 4 * X^1 = 4 * X^0" # 4X = -1
#run_testcase "8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0" # -5.6X^3 - 6X + 5 = 0
#run_testcase "42 * X ^ 0 = 42 * X ^ 0"
#run_testcase "42 * X ^ 2 + 8 * X ^ 1 = 0"
#run_testcase "42 * X ^ 0 = 21 * X ^ 0"


## Evalsheet:

# 0 degree equation after reduction
#run_testcase "5 * X^0 = 5 * X^0"  # Any real number should be a solution
#run_testcase "4 * X^0 = 8 * X^0"  # There is no solution

# First degree equation after reduction
#run_testcase "5 * X ^ 0 = 4 * X ^ 0 + 7 * X ^ 1"

# Second degree equation after reduction - Strictly positive discriminant
#run_testcase "5 * X^0 + 13 * X^1 + 3 * X^2 = 1 * X^0 + 1 * X^1"

# Second degree equation after reduction - Zero discriminant
#run_testcase "6 * X^0 + 11 * X^1 + 5 * X^2 = 1 * X^0 + 1 * X^1"

# Second degree equation after reduction - Strictly negative discriminant
#run_testcase "5 * X^0 + 3 * X^1 + 3 * X^2 = 1 * X^0 + 0 * X^1"

# Third or more degree equation after reduction
run_testcase "6 * X ^ 3 + 5 * X ^ 2 + 6 * X ^ 0 = 0"
#run_testcase "6 * X ^ 3 + 5 * X ^ 2 + 6 * X ^ 0 = 6 * X ^ 3"
