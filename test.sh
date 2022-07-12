#!/usr/bin/env bash

RED=$'\e[1;31m'
GREEN=$'\e[1;32m'
END=$'\e[0m'

EXE="bin/Debug/net6.0/computorv1"

SHOULD_WAIT=$1

function wait_for_keypress() {
    echo "Press any key to continue"
    while true ; do
      read -t 3 -n 1
    if [ $? = 0 ] ; then
      return ;
    fi
    done
}

function run_testcase() {
	EXPECTED_EXIT_STATUS=$1
	EQUATION=$2

	echo "Testing $EQUATION:"
	$EXE "$EQUATION"
	if [[ $? -ne $EXPECTED_EXIT_STATUS ]]; then
	  echo "[${RED}KO${END}] in ${EQUATION}"; echo
	  exit 1
	fi
	echo "[${GREEN}OK${END}] in ${EQUATION}"; echo
	if [[ ${SHOULD_WAIT} == "wait" ]]; then
	  wait_for_keypress
  fi
}

## For Bonus things:
#export COMPUTORV1_BONUS=1

run_testcase 0 "5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0" # -9.3X^2 + 4X + 4 = 0
run_testcase 0 "5 * X^0 + 4 * X^1 = 4 * X^0" # 4X = -1
run_testcase 1 "8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0" # -5.6X^3 - 6X + 5 = 0
run_testcase 0 "42 * X ^ 0 = 42 * X ^ 0"
run_testcase 0 "42 * X ^ 2 + 8 * X ^ 1 = 0"
run_testcase 1 "42 * X ^ 0 = 21 * X ^ 0"


### Evalsheet:

## 0 degree equation after reduction
run_testcase 0 "5 * X^0 = 5 * X^0"  # Any real number should be a solution
run_testcase 1 "4 * X^0 = 8 * X^0"  # There is no solution

# First degree equation after reduction
run_testcase 0 "5 * X ^ 0 = 4 * X ^ 0 + 7 * X ^ 1"

# Second degree equation after reduction - Strictly positive discriminant
run_testcase 0 "5 * X^0 + 13 * X^1 + 3 * X^2 = 1 * X^0 + 1 * X^1"

# Second degree equation after reduction - Zero discriminant
run_testcase 0 "6 * X^0 + 11 * X^1 + 5 * X^2 = 1 * X^0 + 1 * X^1"
run_testcase 0 "3 * X ^ 2 + 24 * X ^ 1 + 48 * X ^ 0 = 0"

# Second degree equation after reduction - Strictly negative discriminant
run_testcase 0 "5 * X^0 + 3 * X^1 + 3 * X^2 = 1 * X^0 + 0 * X^1"

# Third or more degree equation after reduction
run_testcase 1 "6 * X ^ 3 + 5 * X ^ 2 + 6 * X ^ 0 = 0"
run_testcase 0 "6 * X ^ 3 + 5 * X ^ 2 + 6 * X ^ 0 = 6 * X ^ 3"

## Bonus:

# Managing free form entry
run_testcase 0 "X^2 + 1 * X^1 + 4 = 0"  # Perfectly valid
run_testcase 0 "X^2 + 1 * X + 4 * X ^ 0 = 0"  # No ^1
run_testcase 0 "X^2 + X^1 + 4 * X ^ 0 = 0"  # No 1*
run_testcase 0 "X^2 + X^1 + 4 = 0"  # No * X^0
run_testcase 0 "4 - X^2 + X = 0"
run_testcase 0 "X^2 + X = 3"
run_testcase 1 "X^3 + X = X ^ 3"

# Managing entry mistakes
run_testcase 0 "X^2 + X = 0" # Perfectly valid
run_testcase 1 "X^2 + X" # no =sign
run_testcase 1 "X^1 + X = "
run_testcase 0 "X^2 + X = -4" # perfectly valid
run_testcase 1 "X^2l + X = -4"
run_testcase 1 "X^2 + XX = -4"
run_testcase 1 "X^2 + XL = -4"
run_testcase 1 "X^2 + X = -4D"
