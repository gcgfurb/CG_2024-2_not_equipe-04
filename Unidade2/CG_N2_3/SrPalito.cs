using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
namespace gcgcg
{
  internal class SrPalito : Objeto
{
    private float angulo = 0.0f;   // Controla a rotação do Sr. Palito

    public SrPalito(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, new Ponto4D(-0.5, -0.5), new Ponto4D(0.5, 0.5))
    {
    }

    public SrPalito(Objeto _paiRef, ref char _rotulo, Ponto4D ptoIni, Ponto4D ptoFim) : base(_paiRef, ref _rotulo)
    {
        PrimitivaTipo = PrimitiveType.Lines;
        PrimitivaTamanho = 1;

        base.PontosAdicionar(ptoIni);
        base.PontosAdicionar(ptoFim);
        Atualizar();
    }

    private void Atualizar()
    {
        base.ObjetoAtualizar();
    }

    public void Mover(float deslocamento)
    {
        for (int i = 0; i < PontosListaTamanho; i++)
        {
            var ponto = PontosId(i);
            PontosAlterar(new Ponto4D(ponto.X + deslocamento, ponto.Y, ponto.Z), i);
        }
        Atualizar();
    }

    public void AlterarTamanho(float fator)
    {
        for (int i = 0; i < PontosListaTamanho; i++)
        {
            var ponto = PontosId(i);
            PontosAlterar(new Ponto4D(ponto.X * fator, ponto.Y * fator, ponto.Z), i);
        }
        Atualizar();
    }

    public void Girar(float angulo)
    {
        // Acumula o ângulo de rotação
        this.angulo += angulo;

        for (int i = 0; i < PontosListaTamanho; i++)
        {
            var ponto = PontosId(i);
            var pontoNovo = new Ponto4D();

            pontoNovo.X = ponto.X * Math.Cos(this.angulo) - ponto.Y * Math.Sin(this.angulo);
            pontoNovo.Y = ponto.X * Math.Sin(this.angulo) + ponto.Y * Math.Cos(this.angulo);
            pontoNovo.Z = ponto.Z; 
            PontosAlterar(pontoNovo, i);
        }

        Atualizar();
    }
}

}
