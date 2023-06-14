kubectl create namespace otus

helm install nginx ingress-nginx/ingress-nginx --namespace otus -f https://cdn.otus.ru/media/public/20/14/nginx_ingress-25239-20146a.yaml

kubectl create secret generic secret-appsettings  --namespace otus --from-file=./Otus.Microservice.User/appsettings.secrets.json
kubectl create secret generic postgres-secrets --namespace otus --from-literal=POSTGRES_USER="postgres" --from-literal=POSTGRES_PASSWORD="postgres"

kubectl apply -f manifest.yml