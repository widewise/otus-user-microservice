kubectl create namespace monitoring

kubectl config set-context --current --namespace monitoring

helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx/
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update

helm install nginx ingress-nginx/ingress-nginx --namespace monitoring -f nginx-ingress-values.yml --atomic

helm install postgres --namespace monitoring bitnami/postgresql --namespace monitoring -f postgres-values.yml --atomic

kubectl create secret generic secret-appsettings  --namespace monitoring --from-file=./Otus.Microservice.User/appsettings.secrets.json

kubectl apply -f manifest.yml

helm install prometheus prometheus-community/kube-prometheus-stack --namespace monitoring -f prom-values.yml --atomic