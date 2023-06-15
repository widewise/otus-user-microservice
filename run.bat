kubectl create namespace otus

helm install nginx ingress-nginx/ingress-nginx --namespace otus -f https://cdn.otus.ru/media/public/20/14/nginx_ingress-25239-20146a.yaml

helm install postgres --namespace otus bitnami/postgresql --namespace otus -f values.yml

kubectl create secret generic secret-appsettings  --namespace otus --from-file=./Otus.Microservice.User/appsettings.secrets.json

kubectl apply -f manifest.yml