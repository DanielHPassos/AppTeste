# Define a imagem base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Define o diret�rio de trabalho dentro do container
WORKDIR /app

# Copia o arquivo csproj e restaura as depend�ncias
COPY *.csproj ./
RUN dotnet restore

# Copia todo o c�digo-fonte do projeto
COPY . ./

# Compila o projeto
RUN dotnet build -c Release -o out

# Define o novo est�gio para a execu��o
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime-env

# Define o diret�rio de trabalho dentro do container
WORKDIR /app

# Copia os arquivos publicados a partir do est�gio anterior
COPY --from=build-env /app/out .

# Exp�e a porta que a aplica��o est� escutando

EXPOSE 80

# Define o comando de inicializa��o da aplica��o
ENTRYPOINT ["dotnet", "AppTeste.dll"]