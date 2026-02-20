import { useEffect, useState } from 'react'
import axios from 'axios'

function App() {
  // States
  const [products, setProducts] = useState([])
  const [materials, setMaterials] = useState([])
  const [suggestions, setSuggestions] = useState([])
  const [loading, setLoading] = useState(false)
  const [toast, setToast] = useState('')

  // Form States
  const [prodName, setProdName] = useState('')
  const [prodPrice, setProdPrice] = useState('')
  const [matName, setMatName] = useState('')
  const [matQty, setMatQty] = useState('')

  // Recipe States
  const [selectedProductId, setSelectedProductId] = useState('')
  const [selectedMaterialId, setSelectedMaterialId] = useState('')
  const [recipeQuantity, setRecipeQuantity] = useState('')

  const API_BASE = 'https://localhost:7154/api'

  const fetchData = async () => {
    try {
      const [pRes, mRes] = await Promise.all([
        axios.get(`${API_BASE}/Products`),
        axios.get(`${API_BASE}/RawMaterials`)
      ])
      setProducts(pRes.data)
      setMaterials(mRes.data)
    } catch (err) {
      console.error("Data fetch error:", err)
    }
  }

  useEffect(() => { fetchData() }, [])

  const showToast = (msg) => {
    setToast(msg)
    setTimeout(() => setToast(''), 3000)
  }

  // --- FUNÇÕES DE AÇÃO ---

  const handleAddProduct = () => {
    if (!prodName || !prodPrice) return
    axios.post(`${API_BASE}/Products`, { name: prodName, price: parseFloat(prodPrice) })
      .then(() => {
        showToast("Product registered successfully!")
        setProdName(''); setProdPrice(''); fetchData()
      }).catch(() => showToast("Error registering product"))
  }

  const handleAddMaterial = () => {
  if (!matName || !matQty) return;

  // Busca se o nome digitado já existe na sua lista (sem diferenciar maiúsculas/minúsculas)
  const existing = materials.find(m => m.name.toLowerCase() === matName.toLowerCase());

  if (existing) {
    // Se existe, fazemos o PUT enviando o ID no objeto
    axios.put(`${API_BASE}/RawMaterials/${existing.id}`, { 
      id: existing.id, 
      name: existing.name, 
      stockQuantity: parseFloat(matQty) 
    })
    .then(() => { 
      showToast("Stock updated!"); 
      setMatName(''); 
      setMatQty(''); 
      // Espera 500ms para o C# salvar e então atualiza a tela
      setTimeout(() => fetchData(), 500); 
    })
    .catch(() => showToast("Error updating stock"));
  } else {
    // Se é novo, faz o POST normal
    axios.post(`${API_BASE}/RawMaterials`, { 
      name: matName, 
      stockQuantity: parseFloat(matQty) 
    })
    .then(() => { 
      showToast("Material added!"); 
      setMatName(''); 
      setMatQty(''); 
      setTimeout(() => fetchData(), 500); 
    })
    .catch(() => showToast("Error adding material"));
  }
};
  const handleDeleteMaterial = (id) => {
    if (!window.confirm("Delete this material?")) return;
    axios.delete(`${API_BASE}/RawMaterials/${id}`)
      .then(() => { showToast("Material deleted!"); fetchData(); })
      .catch(() => showToast("Error deleting material"));
  };

  const handleLinkMaterial = () => {
    if (!selectedProductId || !selectedMaterialId || !recipeQuantity) {
      alert("Please fill all recipe fields")
      return
    }
    const payload = {
    productId: parseInt(selectedProductId),
    rawMaterialId: parseInt(selectedMaterialId),
    requiredQuantity: parseFloat(recipeQuantity) 
  };
    axios.post(`${API_BASE}/Products/associate-material`, payload)
      .then(() => {
        showToast("Recipe updated!")
        setRecipeQuantity(''); fetchData()
      }).catch(() => showToast("Error linking material"))
  }

  const handleSuggestion = () => {
    setLoading(true)
    axios.get(`${API_BASE}/Products/suggestion`)
      .then(res => setSuggestions(res.data?.suggestedProducts || []))
      .catch(() => showToast("API Suggestion Error"))
      .finally(() => setLoading(false))
  }

  // UI Styles
  const cardStyle = { background: 'rgba(30, 30, 38, 0.8)', padding: '25px', borderRadius: '20px', border: '1px solid rgba(255,255,255,0.1)', boxShadow: '0 10px 30px rgba(0,0,0,0.5)' };
  const inputStyle = { padding: '12px', background: '#0a0a0c', border: '1px solid #333', borderRadius: '10px', color: '#fff', width: '100%', marginBottom: '10px' };
  const labelStyle = { fontSize: '0.7rem', color: '#03dac6', marginBottom: '5px', display: 'block', fontWeight: 'bold' };

  return (
    <div style={{ minHeight: '100vh', width: '100vw', background: '#0a0a0c', color: '#e1e1e6', fontFamily: 'Inter, sans-serif', padding: '40px 0' }}>
      
      {toast && (
        <div style={{ position: 'fixed', top: '20px', right: '20px', background: '#03dac6', color: '#000', padding: '15px 25px', borderRadius: '12px', fontWeight: 'bold', zIndex: 9999 }}>
          {toast}
        </div>
      )}

      <div style={{ width: '90%', maxWidth: '1400px', margin: '0 auto' }}>
        <header style={{ textAlign: 'center', marginBottom: '50px' }}>
          <h1 style={{ fontSize: '3rem', fontWeight: '900', background: 'linear-gradient(45deg, #bb86fc, #03dac6)', WebkitBackgroundClip: 'text', WebkitTextFillColor: 'transparent' }}>AUTOFLEX MANAGER</h1>
          <button onClick={handleSuggestion} style={{ marginTop: '30px', padding: '18px 50px', borderRadius: '50px', background: '#03dac6', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>
            {loading ? 'ANALYZING...' : 'GENERATE PRODUCTION PLAN'}
          </button>
        </header>

        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(350px, 1fr))', gap: '25px' }}>
          
          <section style={cardStyle}>
            <h2 style={{ fontSize: '0.9rem', color: '#bb86fc', marginBottom: '20px' }}>PRODUCT & RECIPE MGMT</h2>
            
            <div style={{ borderBottom: '1px solid #333', marginBottom: '20px', paddingBottom: '20px' }}>
              <span style={labelStyle}>NEW PRODUCT</span>
              <input placeholder="Product Name" style={inputStyle} value={prodName} onChange={e => setProdName(e.target.value)} />
              <input placeholder="Price" type="number" style={inputStyle} value={prodPrice} onChange={e => setProdPrice(e.target.value)} />
              <button onClick={handleAddProduct} style={{ width: '100%', padding: '12px', background: '#bb86fc', color: '#000', borderRadius: '10px', border: 'none', fontWeight: 'bold', cursor: 'pointer' }}>REGISTER PRODUCT</button>
            </div>

            <div>
              <span style={labelStyle}>LINK MATERIAL TO PRODUCT (RECIPE)</span>
              <select style={inputStyle} value={selectedProductId} onChange={e => setSelectedProductId(e.target.value)}>
                <option value="">Select Product</option>
                {products.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
              </select>
              <select style={inputStyle} value={selectedMaterialId} onChange={e => setSelectedMaterialId(e.target.value)}>
                <option value="">Select Material</option>
                {materials.map(m => <option key={m.id} value={m.id}>{m.name}</option>)}
              </select>
              <input placeholder="Qty Needed per unit" type="number" style={inputStyle} value={recipeQuantity} onChange={e => setRecipeQuantity(e.target.value)} />
              <button onClick={handleLinkMaterial} style={{ width: '100%', padding: '12px', background: 'transparent', border: '2px solid #03dac6', color: '#03dac6', borderRadius: '10px', fontWeight: 'bold', cursor: 'pointer' }}>LINK INGREDIENT</button>
            </div>
          </section>

          <section style={{ ...cardStyle, border: '1px solid #03dac6' }}>
            <h2 style={{ fontSize: '0.9rem', color: '#03dac6', marginBottom: '20px' }}>PRODUCTION INTELLIGENCE</h2>
            {suggestions.length > 0 ? suggestions.map((s, i) => (
              <div key={i} style={{ background: '#121214', padding: '20px', borderRadius: '15px', marginBottom: '10px', border: '1px solid #222' }}>
                <div style={{ color: '#888', fontSize: '0.8rem' }}>SUGGESTED PRODUCTION</div>
                <div style={{ fontSize: '1.4rem', fontWeight: 'bold' }}>{s.productName}</div>
                <div style={{ fontSize: '2rem', color: '#03dac6', fontWeight: '900' }}>{s.quantityToProduce} <span style={{fontSize: '1rem'}}>units</span></div>
              </div>
            )) : <p style={{color: '#555', textAlign: 'center', marginTop: '50px'}}>No data generated yet.</p>}
          </section>

          <section style={cardStyle}>
            <h2 style={{ fontSize: '0.9rem', color: '#888', marginBottom: '20px' }}>RAW MATERIALS STOCK</h2>
            <input placeholder="Material Name" style={inputStyle} value={matName} onChange={e => setMatName(e.target.value)} />
            <input placeholder="Current Stock Qty" type="number" style={inputStyle} value={matQty} onChange={e => setMatQty(e.target.value)} />
            <button onClick={handleAddMaterial} style={{ width: '100%', padding: '12px', background: '#555', color: '#fff', borderRadius: '10px', border: 'none', fontWeight: 'bold', cursor: 'pointer', marginBottom: '20px' }}>UPDATE STOCK</button>
            
            <div style={{ maxHeight: '300px', overflowY: 'auto' }}>
              {materials.map(m => (
                <div key={m.id} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '12px', background: 'rgba(255,255,255,0.03)', borderRadius: '10px', marginBottom: '8px' }}>
                  <div>
                    <span style={{display: 'block'}}>{m.name}</span>
                    <span style={{ color: '#03dac6', fontWeight: 'bold' }}>{m.stockQuantity ?? 0} un</span>
                  </div>
                  <button onClick={() => handleDeleteMaterial(m.id)} style={{background: 'none', border: '1px solid #ff4d4d', color: '#ff4d4d', padding: '5px 10px', borderRadius: '5px', cursor: 'pointer', fontSize: '0.7rem'}}>DELETE</button>
                </div>
              ))}
            </div>
          </section>

        </div>
      </div>
    </div>
  )
}

export default App