function loadProducts() {
    fetch('/api/products')
        .then(res => res.json())
        .then(data => {
            const list = document.getElementById('productList');
            list.innerHTML = '';
            data.forEach(p => {
                const li = document.createElement('li');
                li.textContent = `${p.name} - $${p.price}`;
                list.appendChild(li);
            });
        });
}
