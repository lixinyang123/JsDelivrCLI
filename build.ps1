dotnet restore

echo "Publish windows version"
dotnet publish -r win-x64

echo "Publish linux version"
dotnet publish -r linux-x64

echo "Publish osx version"
dotnet publish -r osx-x64