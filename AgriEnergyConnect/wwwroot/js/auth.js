async function login(email, password, role) {
    const response = await fetch(`/auth/${role}-login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    // Handle JWT storage
}