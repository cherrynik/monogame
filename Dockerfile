FROM mcr.microsoft.com/dotnet/sdk:10.0

RUN apt-get update && apt-get install -y --no-install-recommends \
    git \
    libfontconfig1 \
    libfreetype6 \
    libicu-dev \
    libopenal1 \
    libsdl2-2.0-0 \
    xvfb \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
