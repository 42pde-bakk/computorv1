FROM debian:9
WORKDIR /code
## https://docs.microsoft.com/en-us/dotnet/core/install/linux-debian

RUN apt-get update && apt-get upgrade -y
RUN apt-get install -y wget pgp curl man make

## A	dd the Microsoft package signing key to your list of trusted keys and add the package repository
RUN wget -O - https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
	&& mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
	&& wget https://packages.microsoft.com/config/debian/9/prod.list \
	&& mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
	&& chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
	&& chown root:root /etc/apt/sources.list.d/microsoft-prod.list

## Install the SDK
RUN apt-get update; \
  apt-get install -y apt-transport-https && \
  apt-get update -y && \
  apt-get install -y dotnet-sdk-6.0

# Install the runtime
RUN apt-get install -y dotnet-runtime-6.0